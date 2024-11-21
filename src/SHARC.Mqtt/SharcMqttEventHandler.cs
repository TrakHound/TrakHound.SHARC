// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

namespace SHARC.Mqtt
{
    public delegate void SharcMqttEventHandler<TClient>(TClient sharcClient);

    public delegate void SharcMqttEventHandler<TClient, TData>(TClient sharcClient, TData data);
}
