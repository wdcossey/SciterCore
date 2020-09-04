namespace SciterCore
{
    public struct ScriptEventResult
    {
        public bool IsSuccessful { get; }
			
        public SciterValue Value { get; }
        
        internal ScriptEventResult(bool isSuccessful, SciterValue value)
        {
            IsSuccessful = isSuccessful;
            Value = value;
        }

        public static ScriptEventResult Successful(SciterValue value = null)
        {
            return new ScriptEventResult(true, value);
        }
        
        public static ScriptEventResult Failed()
        {
            return new ScriptEventResult(false, null);
        }
        
    }
}