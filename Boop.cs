using System;

public class Fingers
{
  private static int ScrollAmount = 425;

  bool useRightHand = false;

  public Fingers()
  {
    LoopListener loop = new LoopListener(this);

    // Keep this process running
    Console.ReadLine();
  }

  public void HandleLoopEvent(LoopButton b, Boolean pressed)
  {
    Console.WriteLine("{0} {1}", b, pressed ? "pressed" : "released");
    LoopButton fwdButton = (useRightHand ? LoopButton.FWD : LoopButton.BACK);
    LoopButton backButton = (useRightHand ? LoopButton.BACK : LoopButton.FWD);

    if (b == LoopButton.CENTER && pressed)
    {
      Winput.SendKeyCombo(new Winput.VK[] {Winput.VK.CONTROL, Winput.VK.D});
    }
    else if (b == LoopButton.CENTER && !pressed)
    {
      //Winput.MouseButton(Winput.MouseEventF.LeftUp);
    }
    else if (b == fwdButton && pressed)
    {
      Winput.SendKeyCombo(new Winput.VK[] {Winput.VK.CONTROL, Winput.VK.TAB});
    }
    else if (b == fwdButton && !pressed)
    {
      //Winput.MouseButton(Winput.MouseEventF.RightUp);
    }
    else if (b == backButton && pressed)
    {
      Winput.SendKeyCombo(new Winput.VK[] {Winput.VK.CONTROL, Winput.VK.LSHIFT, Winput.VK.TAB});
    }
    else if (b == backButton && !pressed)
    {
     //Winput.MouseButton(Winput.MouseEventF.RightUp);
    }
    else if (b == LoopButton.DOWN && pressed)
    {
      Winput.ScrollMouse(-ScrollAmount);
    }
    else if (b == LoopButton.UP && pressed)
    {
      Winput.ScrollMouse(ScrollAmount);
    }
  }

  public static void Main()
  {
    new Fingers();
  }
}