using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.Models;
using VirtualSales.Core.Models.Annotations;

namespace VirtualSales.Core.AppServices
{
    public interface ISharedDataService
    {
        PersonModel Person { get; }
        MeetingStateModel MeetingState { get; }
        AnnotationsModel AnnotationsModel { get; }
    }

    class SharedDataService : ISharedDataService
    {
        public SharedDataService()
        {
            Person = new PersonModel();
            MeetingState = new MeetingStateModel();
            AnnotationsModel = new AnnotationsModel();
        }

        public AnnotationsModel AnnotationsModel { get; private set; }
        public PersonModel Person { get; private set; }
        public MeetingStateModel MeetingState { get; private set; }
    }
}
