using Chatapp.Application.Users.CommandSide.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using NpgsqlTypes;
using Chatapp.Core.Extensions;
using Chatapp.Repository.Base;
using Chatapp.Core.Helpers;

namespace Chatapp.Repository.Context
{
    public class ChatappContext : DbContext
    {
        protected bool IgnoreMappingToDerivedContextMigrations { get; set; }
        public DbSet<User> UserTb { get; set; }

        private static readonly List<Tuple<Func<Type, bool>, NpgsqlDbType>> _typeMappings = new List<Tuple<Func<Type, bool>, NpgsqlDbType>>
        {
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(string), NpgsqlDbType.Varchar),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(int), NpgsqlDbType.Integer),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(decimal), NpgsqlDbType.Numeric),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(DateTime), NpgsqlDbType.Timestamp),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(bool), NpgsqlDbType.Boolean),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(byte), NpgsqlDbType.Smallint),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(short), NpgsqlDbType.Smallint),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(long), NpgsqlDbType.Bigint),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(float), NpgsqlDbType.Real),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(double), NpgsqlDbType.Double),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(byte[]), NpgsqlDbType.Bytea),
            Tuple.Create<Func<Type, bool>, NpgsqlDbType>(t => t == typeof(object), NpgsqlDbType.Jsonb)
        };

        public ChatappContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (IgnoreMappingToDerivedContextMigrations)
            {
                ExcludeMigrations(modelBuilder);

                GetNameHiloSequences().ToList().ForEach(sequenceName => modelBuilder.Model.RemoveSequence(sequenceName));

                return;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatappContext).Assembly);

            base.OnModelCreating(modelBuilder);

            ConfigureConventions(modelBuilder);
        }
        protected static Action<ModelBuilder> ExcludeMigrations => (modelBuilder) =>
        {
            var types = typeof(ChatappContext).GetProperties();

            foreach (var type in types)
            {
                var arguments = type.PropertyType.GenericTypeArguments;

                if (arguments.Length == 0)
                {
                    continue;
                }

                if (arguments[0].Name.ToLower().EndsWith("tb"))
                {
                    modelBuilder.Entity(arguments[0]).ToTable(arguments[0].Name.ToSnakeCase(), t => t.ExcludeFromMigrations());
                }

                modelBuilder.Ignore(arguments[0]);
            }
        };
        protected static IEnumerable<string> GetNameHiloSequences()
        {
            return typeof(ChatappContext).Assembly.GetInstancesFromAssembly<IHiloSequence>(typeof(IHiloSequence)).Select(item => item.GetSequenceName());
        }
        protected static Action<ModelBuilder> ConfigureConventions => modelBuilder =>
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (!entity.ClrType.BaseType.Name.Contains("ValueObject") && string.IsNullOrEmpty(entity.GetViewName()))
                {
                    entity.SetTableName(entity.GetTableName().ToSnakeCase());
                }

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToSnakeCase());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                foreach (var index in entity.GetProperties().Where(p => (p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)) &&
                                                                        string.IsNullOrEmpty(p.GetColumnType())))
                {
                    index.SetColumnType("numeric(18,2)");
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }
        };
        public NpgsqlConnection GetConnection() => Database.GetDbConnection() as NpgsqlConnection;
        public IDbContextTransaction CurrentTransaction() => Database.CurrentTransaction;
        internal static NpgsqlDbType GetNpgsqlDbType(Type type)
        {
            var mapping = _typeMappings.FirstOrDefault(m => m.Item1(type)) ?? throw new ArgumentException($"Type {type.Name} is not supported");
            return mapping.Item2;
        }
    }
}
