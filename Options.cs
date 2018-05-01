using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mh.Twitter.Resetter
{
    class Options
    {
        public enum Opt
        {
            None,
            Erase,
            Kick,
            All
        }

        public bool silence { get; private set; }
        public Opt opt { get; private set; }

        public static Options ParseArgs(string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidArgumentException("Not exist argument.");
            }

            Options options = new Options();
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-e":
                    case "--erase":
                        if (options.opt != Opt.None)
                            throw new InvalidArgumentException("The other option was already set.");
                        options.opt = Opt.Erase;
                        break;

                    case "-k":
                    case "--kick":
                        if (options.opt != Opt.None)
                            throw new InvalidArgumentException("The other option was already set.");
                        options.opt = Opt.Kick;
                        break;

                    case "-a":
                    case "--all":
                        if (options.opt != Opt.None)
                            throw new InvalidArgumentException("The other option was already set.");
                        options.opt = Opt.All;
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
