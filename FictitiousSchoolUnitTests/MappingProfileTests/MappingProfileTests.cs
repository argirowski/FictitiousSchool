using Application.DTOs;
using Application.Mapping;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FictitiousSchoolUnitTests.MappingProfileTests
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MappingProfile_ConfigurationIsValid()
        {
            // Arrange & Act
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Assert
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_ParticipantDTO_To_Participant()
        {
            // Arrange
            var participantDTO = new ParticipantDTO { Name = "John Doe", Phone = "1234567890", Email = "john.doe@example.com" };

            // Act
            var participant = _mapper.Map<Participant>(participantDTO);

            // Assert
            Assert.Equal(participantDTO.Name, participant.Name);
            Assert.Equal(participantDTO.Phone, participant.Phone);
            Assert.Equal(participantDTO.Email, participant.Email);
        }

        [Fact]
        public void Should_Map_CompanyDTO_To_Company()
        {
            // Arrange
            var companyDTO = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" };

            // Act
            var company = _mapper.Map<Company>(companyDTO);

            // Assert
            Assert.Equal(companyDTO.Name, company.Name);
            Assert.Equal(companyDTO.Phone, company.Phone);
            Assert.Equal(companyDTO.Email, company.Email);
        }

        [Fact]
        public void Should_Map_CourseDTO_To_Course()
        {
            // Arrange
            var courseDTO = new CourseDTO { Id = 1, Name = "Course 1" };

            // Act
            var course = _mapper.Map<Course>(courseDTO);

            // Assert
            Assert.Equal(courseDTO.Id, course.Id);
            Assert.Equal(courseDTO.Name, course.Name);
        }

        [Fact]
        public void Should_Map_CourseDateDTO_To_CourseDate()
        {
            // Arrange
            var courseDateDTO = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.Now };

            // Act
            var courseDate = _mapper.Map<CourseDate>(courseDateDTO);

            // Assert
            Assert.Equal(courseDateDTO.Id, courseDate.Id);
            Assert.Equal(courseDateDTO.Date, courseDate.Date);
        }

        [Fact]
        public void Should_Map_FictitiousSchoolApplicationDTO_To_FictitiousSchoolApplication()
        {
            // Arrange
            var applicationDTO = new FictitiousSchoolApplicationDTO
            {
                Id = Guid.NewGuid(),
                Course = new CourseDTO { Id = 1, Name = "Course 1" },
                CourseDate = new CourseDateDTO { Id = Guid.NewGuid(), Date = DateTime.Now },
                Company = new CompanyDTO { Name = "Company 1", Phone = "1234567890", Email = "company1@example.com" },
                Participants = new List<ParticipantDTO>
                {
                    new ParticipantDTO { Name = "John Doe", Phone = "1234567890", Email = "john.doe@example.com" }
                }
            };

            // Act
            var application = _mapper.Map<FictitiousSchoolApplication>(applicationDTO);

            // Assert
            Assert.Equal(applicationDTO.Id, application.Id);
            Assert.Equal(applicationDTO.Course.Id, application.Course.Id);
            Assert.Equal(applicationDTO.Course.Name, application.Course.Name);
            Assert.Equal(applicationDTO.CourseDate.Id, application.CourseDate.Id);
            Assert.Equal(applicationDTO.CourseDate.Date, application.CourseDate.Date);
            Assert.Equal(applicationDTO.Company.Name, application.Company.Name);
            Assert.Equal(applicationDTO.Company.Phone, application.Company.Phone);
            Assert.Equal(applicationDTO.Company.Email, application.Company.Email);
            Assert.Equal(applicationDTO.Participants[0].Name, application.Participants.First().Name);
            Assert.Equal(applicationDTO.Participants[0].Phone, application.Participants.First().Phone);
            Assert.Equal(applicationDTO.Participants[0].Email, application.Participants.First().Email);
        }
    }
}