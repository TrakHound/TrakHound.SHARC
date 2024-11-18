namespace SHARC.Mqtt
{
    public delegate void SharcMqttEventHandler<TClient>(TClient sharcClient);

    public delegate void SharcMqttEventHandler<TClient, TData>(TClient sharcClient, TData data);
}
