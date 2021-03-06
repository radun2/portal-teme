﻿using PortalTeme.API.Models.Assignments;
using PortalTeme.Data.Models;
using PortalTeme.Data.Models.Assignments.Projections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortalTeme.API.Mappers {

    public interface IAssignmentMapper {
        AssignmentDTO MapAssignment(Assignment assignment);
        Assignment MapAssignmentEditDTO(AssignmentEditDTO assignment);

        AssignmentEntryDTO MapAssignmentEntryProjection(AssignmentEntryProjection entry);
    }

    public class AssignmentMapper : IAssignmentMapper {
        private readonly ICourseMapper courseMapper;

        public AssignmentMapper(ICourseMapper courseMapper) {
            this.courseMapper = courseMapper;
        }

        public AssignmentDTO MapAssignment(Assignment assignment) {
            return new AssignmentDTO {
                Id = assignment.Id,
                Course = courseMapper.MapCourseEdit(assignment.Course),
                Name = assignment.Name,
                Slug = assignment.Slug,
                Description = assignment.Description,

                DateAdded = assignment.DateAdded,
                StartDate = assignment.StartDate,
                LastUpdated = assignment.LastUpdated,
                EndDate = assignment.EndDate,
            };
        }

        public Assignment MapAssignmentEditDTO(AssignmentEditDTO assignment) {
            return new Assignment {
                Id = assignment.Id ?? Guid.Empty,
                Course = new Course {
                    Id = assignment.Course.Id
                },
                Name = assignment.Name,
                Description = assignment.Description,

                DateAdded = assignment.DateAdded,
                StartDate = assignment.StartDate,
                LastUpdated = assignment.LastUpdated,
                EndDate = assignment.EndDate
            };
        }

        public AssignmentEntryDTO MapAssignmentEntryProjection(AssignmentEntryProjection entry) {
            return new AssignmentEntryDTO {
                Id = entry.Id,
                AssignmentId = entry.AssignmentId,
                CourseId = entry.CourseId,
                StudentId = entry.StudentId,
                State = entry.State,
                Grading = entry.Grading,
                Versions = entry.Versions.Select(version => MapAssignmentEntryVersion(version)).ToList()
            };

        }

        private AssignmentEntryVersionDTO MapAssignmentEntryVersion(AssignmentEntryVersion entryVersion) {
            return new AssignmentEntryVersionDTO {
                Id = entryVersion.Id,
                DateAdded = entryVersion.DateAdded,
                Files = entryVersion.Files.Select(file => MapAssignmentEntryFile(file)).ToList()
            };
        }

        private AssignmentEntryFileDTO MapAssignmentEntryFile(AssignmentEntryFile file) {
            return new AssignmentEntryFileDTO {
                Id = file.Id,
                Name = file.Name,
                Description = file.Description,
                FileType = file.FileType
            };
        }
    }
}
