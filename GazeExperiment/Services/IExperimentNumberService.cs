using System.Threading.Tasks;

namespace GazeExperiment
{
    public interface IExperimentNumberService
    {
        int number { get;  }

       void increaseNumber();
    }
}