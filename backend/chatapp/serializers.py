from rest_framework import serializers

from .models import User, Message


class UserSerializer(serializers.ModelSerializer):
    messages = serializers.HyperlinkedIdentityField(view_name='usermessage-list', lookup_field='username')

    class Meta:
        model = User
        fields = ('id', 'username', 'first_name', 'last_name', 'messages', )


class MessageSerializer(serializers.ModelSerializer):
    sender = UserSerializer(required=True)
    receiver = UserSerializer(required=True)

    def get_validation_exclusions(self, *args, **kwargs):
        # Need to exclude `user` since we'll add that later based off the request
        exclusions = super(MessageSerializer, self).get_validation_exclusions(*args, **kwargs)
        return exclusions + ['sender', 'receiver']

    class Meta:
        model = Message
        fields = '__all__'