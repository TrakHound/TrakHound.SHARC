namespace SHARC.Mqtt
{
    public delegate void SharcMqttEventHandler<TData>(string sharcId, TData data);
}
