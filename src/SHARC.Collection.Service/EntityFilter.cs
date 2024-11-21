// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

using TrakHound;
using TrakHound.Entities;

namespace SHARC.Collection
{
    internal static class EntityFilter
    {
        private static readonly Dictionary<string, string> _sentItems = new Dictionary<string, string>();
        private static readonly object _lock = new object();


        public static bool Add(string key, object value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                lock (_lock)
                {
                    var newItem = value?.ToString().ToMD5Hash();

                    var existingItem = _sentItems.GetValueOrDefault(key);
                    if (existingItem == null || existingItem != newItem)
                    {
                        _sentItems.Remove(key);
                        _sentItems.Add(key, newItem);
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool Add(ITrakHoundObjectEntity entity)
        {
            return Add($"OBJECT|{entity.Uuid}", $"{entity.DefinitionUuid}:{entity.Priority}");
        }

        public static bool Add(ITrakHoundObjectMetadataEntity entity)
        {
            return Add(entity.Uuid, $"{entity.DefinitionUuid}:{entity.ValueDefinitionUuid}:{entity.Value}");
        }

        public static bool Add(ITrakHoundObjectNumberEntity entity)
        {
            return Add(entity.ObjectUuid, entity.Value);
        }

        public static bool Add(ITrakHoundObjectObservationEntity entity)
        {
            return Add(entity.ObjectUuid, entity.Value);
        }

        public static bool Add(ITrakHoundObjectSetEntity entity)
        {
            return Add(entity.Uuid, entity.Value);
        }

        public static bool Add(ITrakHoundObjectStringEntity entity)
        {
            return Add(entity.ObjectUuid, entity.Value);
        }

        public static bool Add(ITrakHoundObjectTimestampEntity entity)
        {
            return Add(entity.ObjectUuid, entity.Value);
        }


        public static bool Add(ITrakHoundDefinitionEntity entity)
        {
            return Add(entity.Uuid, entity.ParentUuid);
        }

        public static bool Add(ITrakHoundDefinitionDescriptionEntity entity)
        {
            return Add(entity.Uuid, entity.Text);
        }

        public static bool Add(ITrakHoundDefinitionMetadataEntity entity)
        {
            return Add(entity.Uuid, entity.Value);
        }
    }
}
