using System.Collections;
using UnityEngine;

namespace DesignPatterns
{
    /*
    Command Design Pattern là một mẫu thiết kế mà mục tiêu chính là 
    chuyển đối tượng yêu cầu thành một đối tượng chứa tất cả thông tin cần thiết để thực hiện yêu cầu. 
    */

    // Command Interface
    public interface ICommand
    {
        void Execute();
    }

    // Receiver (đối tượng thực hiện yêu cầu)
    public class Light
    {
        public void TurnOn()
        {
            Debug.Log("Light is ON");
        }

        public void TurnOff()
        {
            Debug.Log("Light is OFF");
        }
    }

    // Concrete Command
    public class TurnOnCommand : ICommand
    {
        private Light light;

        public TurnOnCommand(Light light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.TurnOn();
        }
    }

    // Concrete Command
    public class TurnOffCommand : ICommand
    {
        private Light light;

        public TurnOffCommand(Light light)
        {
            this.light = light;
        }

        public void Execute()
        {
            light.TurnOff();
        }
    }

    // Invoker (người gọi yêu cầu)
    public class RemoteControl
    {
        private ICommand command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public void PressButton()
        {
            command.Execute();
        }
    }


    public class ExampleCommand
    {
        public void Example()
        {
            // Sử dụng Command Design Pattern
            Light light = new Light();
            ICommand turnOnCommand = new TurnOnCommand(light);
            ICommand turnOffCommand = new TurnOffCommand(light);

            RemoteControl remote = new RemoteControl();

            remote.SetCommand(turnOnCommand);
            remote.PressButton(); // In ra "Light is ON"

            remote.SetCommand(turnOffCommand);
            remote.PressButton(); // In ra "Light is OFF"
        }
    }
}