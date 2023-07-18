using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group_Project.Models;

namespace Group_Project.Messenger
{
    public class MessengerC : ValueChangedMessage<DoctorC>
    {
        public MessengerC(DoctorC value) : base(value) { }
    }

    public class MessengerCfirst : ValueChangedMessage<DoctorC>
    {
        public MessengerCfirst(DoctorC value) : base(value) { }
    }

    public class MessengerOverviewDoc : ValueChangedMessage<DoctorC>
    {
        public MessengerOverviewDoc(DoctorC value) : base(value) { }
    }

    public class MessengerPatientToEdit : ValueChangedMessage<Patient>
    {
        public MessengerPatientToEdit(Patient value) : base(value) { }
    }

    public class MessengerPatientToEditFirst : ValueChangedMessage<Patient>
    {
        public MessengerPatientToEditFirst(Patient value) : base(value) { }
    }

    public class MessengerDoctorToAdd : ValueChangedMessage<User>
    {
        public MessengerDoctorToAdd(User value) : base(value) { }
    }

    public class MessengerDoctorToAddFirst : ValueChangedMessage<User>
    {
        public MessengerDoctorToAddFirst(User value) : base(value) { }
    }

    public class MessengerPatientOfDoctorToEdit : ValueChangedMessage<Patient>
    {
        public MessengerPatientOfDoctorToEdit(Patient value) : base(value) { }
    }

    public class MessengerPatientOfDoctorToEditFirst : ValueChangedMessage<Patient>
    {
        public MessengerPatientOfDoctorToEditFirst(Patient value) : base(value) { }
    }

    public class MessengerSendBackUpdatedDoctor : ValueChangedMessage<DoctorC>
    {
        public MessengerSendBackUpdatedDoctor(DoctorC value) : base(value) { }
    }
}
