using Vortice.XInput;

namespace USBWeelToWeb;

internal class Program
{

    private static void ClearLine()
    {
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }

    static void Main(string[] args)
    {


        DIInputDevice directInputDevice = new DIInputDevice();
        directInputDevice.Initialize(IntPtr.Zero);


        int userIndex = 0;
        int[] searchArray = new int[4] { -1, -1, -1, -1 };

        while (true)
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).KeyChar)
                {
                    case 'q':
                        return;

                    case 'x':
                        XInputSelect(searchArray.Where(x => x >=0).ToArray());
                        break;

                    case 'd':
                        directInputDevice.GetKJoystickUpdates();
                        break;
                                       
                }
            }

            //directInputDevice.GetKeyboardUpdates();
            //directInputDevice.GetKJoystickUpdates();

            

            for (int i = 0; i < searchArray.Length; i++) 
            {
                bool ok = XInput.GetState(i, out State state);

                if (ok)
                {
                    Vibration vibration = new((ushort)(state.Gamepad.LeftTrigger << 8), (ushort)(state.Gamepad.RightTrigger << 8));
                    XInput.SetVibration(i, vibration);
                    searchArray[i] = i;
                }
            }
            
            Console.Clear();

            if (!searchArray.Where(x => x >= 0).Any())
            {
                Console.SetCursorPosition(0, 0);

                ClearLine(); Console.WriteLine($"NO XInput devices available");
                ClearLine(); Console.WriteLine($"=========================================================================\n");

            }
            else if (searchArray.Where(x => x >= 0).Any())
            {
                ClearLine(); Console.WriteLine($"Available XInput joyskicks is {searchArray.Where(x => x >= 0).Count()}");
                ClearLine(); Console.WriteLine($"=========================================================================");
                ClearLine(); Console.WriteLine($"Press 'x' to see if data available\n");
                
            }
            
            if (directInputDevice.AvailableJoysticks == 0)
            {
                ClearLine(); Console.WriteLine($"NO DInput joysticks available");
                ClearLine(); Console.WriteLine($"=========================================================================");
            }
            else if (directInputDevice.AvailableJoysticks >= 0)
            {
                ClearLine(); Console.WriteLine($"Available DInput joyskicks is {directInputDevice.AvailableJoysticks}");
                ClearLine(); Console.WriteLine($"=========================================================================");
                ClearLine(); Console.WriteLine($"Press 'd' to see if data available");
            }


             Thread.Sleep(10);
        }

        static void XInputSelect(int[] userIndexArray)
        {

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case 'q':
                            return;
                    }
                }

                foreach (int userIndex in userIndexArray)
                {
                    bool ok = XInput.GetState(userIndex, out State state);

                    if (ok)
                    {
                        Vibration vibration = new((ushort)(state.Gamepad.LeftTrigger << 8), (ushort)(state.Gamepad.RightTrigger << 8));
                        XInput.SetVibration(userIndex, vibration);
                    }
                    else
                    {
                        state = new State();    // empty state variable if GetState failed
                    }

                    Console.SetCursorPosition(0, 0);

                    ClearLine(); Console.WriteLine($"=========================================================================");
                    ClearLine(); Console.WriteLine($"Information about connected XInput joystick, press 'q' to quit..");
                    ClearLine(); Console.WriteLine($"=========================================================================");
                    ClearLine(); Console.WriteLine($"Gamepad       : {userIndex + 1} {(ok ? "(ok)" : "(not ok)")}");
                    ClearLine(); Console.WriteLine($"Buttons       : {state.Gamepad.Buttons}");
                    ClearLine(); Console.WriteLine($"Left Thumb    : X = {state.Gamepad.LeftThumbX} Y = {state.Gamepad.LeftThumbY}");
                    ClearLine(); Console.WriteLine($"Left Trigger  : {state.Gamepad.LeftTrigger}");
                    ClearLine(); Console.WriteLine($"Right Thumb   : X = {state.Gamepad.RightThumbX} Y = {state.Gamepad.RightThumbY}");
                    ClearLine(); Console.WriteLine($"Right Trigger : {state.Gamepad.RightTrigger}");
                    Console.WriteLine();
                }
            }                  
        }
    }
}
