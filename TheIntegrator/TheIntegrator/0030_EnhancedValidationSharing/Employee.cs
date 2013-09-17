using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TheIntegrator.Annotations;

namespace TheIntegrator._0030_EnhancedValidationSharing
{
    public class Employee : BaseEntity
    {
        private string _email;
        private DateTime? _joinDate;
        private DateTime? _leaveDate;
        private decimal _salary;
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public decimal Salary
        {
            get { return _salary; }
            set
            {
                if (value == _salary) return;
                _salary = value;
                OnPropertyChanged();
            }
        }

        public DateTime? LeaveDate
        {
            get { return _leaveDate; }
            set
            {
                if (value.Equals(_leaveDate)) return;
                _leaveDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? JoinDate
        {
            get { return _joinDate; }
            set
            {
                if (value.Equals(_joinDate)) return;
                _joinDate = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged();
            }
        }


    }
}
