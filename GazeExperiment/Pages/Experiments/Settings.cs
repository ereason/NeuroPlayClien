using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSharedClasses.Models; 

namespace GazeExperiment.Pages.Experiments
{
    class Settings
    {
        public int countIteration;
        public double durationShow;
        public double durationPause;

        public Settings(int n, double s, double p)
        {
            countIteration = n;
            durationShow = s;
            durationPause = p;
        }

        public static Settings getSettings(UserType type)
        {
            switch (type)
            {
                case UserType.Elementary:
                    return new Settings(50, 3, 2.5);
                case UserType.Experienced:
                    return new Settings(60, 2.5, 2.5);
                case UserType.Advanced:
                    return new Settings(75, 2, 1.5);
                default:
                    return null;
            }
        }
    }
}
