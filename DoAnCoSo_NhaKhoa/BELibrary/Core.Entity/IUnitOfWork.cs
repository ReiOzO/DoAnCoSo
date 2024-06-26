﻿using BELibrary.Core.Entity.Repositories;
using System;

namespace BELibrary.Core.Entity
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        ICategoryRepository Categories { get; }
        IServiceRepository Services { get; }
        IDetailRecordRepository DetailRecords { get; }
        IPatientRepository Patients { get; }
        IRecordRepository Records { get; }
        IDoctorRepository Doctors { get; }
        IFacultyRepository Faculties { get; }
        IPatientRecordRepository PatientRecords { get; }
        IUserVerificationRepository UserVerifications { get; }
        IDoctorScheduleRepository DoctorSchedules { get; }
        IPatientDoctorRepository PatientDoctors { get; }
        IArticleRepository Articles { get; }

        int Complete();
    }
}