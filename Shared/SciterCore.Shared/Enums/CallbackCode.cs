namespace SciterCore
{
    public enum CallbackCode
    {
        Undefined = -1,
        LoadData = 0x01,
        DataLoaded = 0x02,
        AttachBehavior = 0x04,
        EngineDestroyed = 0x05,
        PostedNotification = 0x06,
        GraphicsCriticalFailure = 0x07,
    }
}