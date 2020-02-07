using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
public delegate int ExampleCallback(int documentsimported);
namespace Finalaplication.Models
{
    public class ThreadWithState
    {
        // State information used in the task.
        private IMongoCollection<Volunteer> name;
       private List<string[]> result;
        private string duplicates;
        private int documentsimported;


        // Delegate used to execute the callback method when the
        // task is complete.
        private ExampleCallback callback;

        // The constructor obtains the state information and the
        // callback delegate.
        public ThreadWithState(IMongoCollection<Volunteer> name, List<string[]> result, string duplicates, int documentsimported, ExampleCallback callbackDelegate)
        {
           this.name = name;
            this.result=result;
            this.duplicates = duplicates;
            this.documentsimported = documentsimported;
            callback = callbackDelegate;
        }

        public void ThreadProc()
        {
            callback?.Invoke(1);
        }


    }
}

