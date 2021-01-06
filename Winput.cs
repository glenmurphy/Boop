// From https://github.com/Bojidarist/SendInputsDemo
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class Winput
{
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)] public MouseInput mi;
        [FieldOffset(0)] public KeyboardInput ki;
        [FieldOffset(0)] public HardwareInput hi;
    }

    public struct Input
    {
        public int type;
        public InputUnion u;
    }

    [Flags]
    public enum InputType
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }

    [Flags]
    public enum KeyEventF
    {
        KeyDown = 0x0000,
        ExtendedKey = 0x0001,
        KeyUp = 0x0002,
        Unicode = 0x0004,
        Scancode = 0x0008
    }

    // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    // TODO: investigate using https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.keyinterop.virtualkeyfromkey?view=net-5.0 instead
    [Flags]
    public enum VK
    {
        TAB = 0x09,
        SHIFT = 0x10,
        CONTROL = 0x11,
        PRIOR = 0x21,
        NEXT = 0x22,
        D = 0x44,
        LSHIFT = 0xA0,
        RSHIFT = 0xA1,
    }

    [Flags]
    public enum MouseEventF
    {
        Absolute = 0x8000,
        HWheel = 0x01000,
        Move = 0x0001,
        MoveNoCoalesce = 0x2000,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        RightDown = 0x0008,
        RightUp = 0x0010,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        VirtualDesk = 0x4000,
        Wheel = 0x0800,
        XDown = 0x0080,
        XUp = 0x0100
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, int cButtons, uint dwExtraInfo);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

    [DllImport("user32.dll")]
    public static extern IntPtr GetMessageExtraInfo();

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    [DllImport("User32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    public static POINT GetCursorPosition()
    {
        GetCursorPos(out POINT point);
        return point;
    }

    public static void SetCursorPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

    public static void SendKeyboardInput(KeyboardInput[] kbInputs)
    {
        Input[] inputs = new Input[kbInputs.Length];

        for (int i = 0; i < kbInputs.Length; i++)
        {
            inputs[i] = new Input
            {
                type = (int)InputType.Keyboard,
                u = new InputUnion
                {
                    ki = kbInputs[i]
                }
            };
        }

        SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
    }

    public static void SendMouseInput(MouseInput[] mInputs)
    {
        Input[] inputs = new Input[mInputs.Length];

        for (int i = 0; i < mInputs.Length; i++)
        {
            inputs[i] = new Input
            {
                type = (int)InputType.Mouse,
                u = new InputUnion
                {
                    mi = mInputs[i]
                }
            };
        }

        SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
    }

    public static void ScrollMouse(int amount)
    {
        mouse_event((uint)MouseEventF.Wheel, 0, 0, amount, 0);
    }

    public static void MouseButton(MouseEventF button)
    {
        mouse_event((uint)button, 0, 0, 0, 0);
    }

    public static void SendKeyCombo(VK[] codes) {
        KeyboardInput[] inputs = new KeyboardInput[codes.Length * 2];

        for (int i = 0; i < codes.Length; i++) {
            inputs[i] = new KeyboardInput
            {
                wVk = (ushort)codes[i],
                dwFlags = 0,
                dwExtraInfo = GetMessageExtraInfo()
            };
        }

        for (int i = 0; i < codes.Length; i++) {
            inputs[i + codes.Length] = new KeyboardInput
            {
                wVk = (ushort)codes[i],
                dwFlags = (uint)KeyEventF.KeyUp,
                dwExtraInfo = GetMessageExtraInfo()
            };
        }

        SendKeyboardInput(inputs);
    }
}