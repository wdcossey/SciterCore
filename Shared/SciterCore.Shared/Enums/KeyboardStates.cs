using System;

namespace SciterCore
{
    [Flags]
    public enum KeyboardStates : int
    {
        ControlKeyPressed = 0x1,
        ShiftKeyPressed = 0x2,
        AltKeyPressed = 0x4
    }
}