using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if (args.Length == 0)
            {
                throw new InvalidArgumentException("Not exist argument.");
            }

            Args options = new Args();
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
                }
            }
            return options;
        }
    }
}
