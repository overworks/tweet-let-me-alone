using System;

namespace Mh.Twitter.Resetter
{
    class Args
    {
        public enum Command
        {
            None,
            Erase,
            Kick,
            All
        }

        public bool silence
        {
            get;
            private set;
        }

        public bool consumer
        {
            get;
            private set;
        }

        public Command command
        {
            get
            {
                return _command;
            }
            private set
            {
                if (_command != Command.None)
                    throw new InvalidArgumentException("Command was already set.");

                _command = value;
            }
        }

        private Command _command;

        public static Args ParseArgs(string[] args)
        {
            Args options = new Args();

            if (args.Length == 0)
            {
                Command command = ReceiveCommand();
                if (command == Command.None)
                    throw new InvalidArgumentException("Not exist argument.");

                options.command = command;
            }
            else
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "erase":
                            options.command = Command.Erase;
                            break;

                        case "kick":
                            options.command = Command.Kick;
                            break;

                        case "all":
                            options.command = Command.All;
                            break;

                        case "-s":
                        case "--silence":
                            options.silence = true;
                            break;

                        case "-c":
                        case "--consumer":
                            options.consumer = true;
                            break;
                    }
                }
            }
            return options;
        }

        private static Command ReceiveCommand()
        {
            Console.Write("Input command (kick/erase/all) ");

            string command = Console.ReadLine();
            switch (command)
            {
                case "erase":
                    return Command.Erase;

                case "kick":
                    return Command.Kick;

                case "all":
                    return Command.All;
            }

            return Command.None;
        }

    }
}
