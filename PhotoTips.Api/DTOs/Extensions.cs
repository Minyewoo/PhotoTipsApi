using System.Linq;
using PhotoTips.Core.Models;

namespace PhotoTips.Api.DTOs
{
    public static class Extensions
    {
        public static SubmissionAdminDto ToAdminDto(this Submission submission) => new SubmissionAdminDto
        {
            Id = submission.Id,
            ModuleEntryId = submission.ModuleEntry.Id,
            Photo = submission.Photo.ToDto(),
            Status = submission.Status,
            Mark = submission.Mark,
            Comment = submission.Comment,
            Time = submission.Time
        };
        public static SubmissionDto ToDto(this Submission submission) => new SubmissionDto
        {
            Id = submission.Id,
            ModuleEntryId = submission.ModuleEntry.Id,
            PhotoId = submission.Photo.Id,
            Status = submission.Status,
            Mark = submission.Mark,
            Comment = submission.Comment,
            Time = submission.Time
        };
        
        public static CityDto ToDto(this City city) =>
            new CityDto
            {
                Id = city.Id,
                Name = city.Name,
            };

        public static LectureContentDto ToDto(this LectureContent lectureContent) =>
            new LectureContentDto
            {
                Id = lectureContent.Id,
                IndexNumber = lectureContent.IndexNumber,
                Content = lectureContent.Content,
                Type = lectureContent.Type,
            };

        public static ModuleEntryDto ToDto(this ModuleEntry moduleEntry) =>
            new ModuleEntryDto
            {
                Id = moduleEntry.Id,
                Name = moduleEntry.Name,
                IndexNumber = moduleEntry.IndexNumber,
                Description = moduleEntry.Description,
                AdditionalInfo = moduleEntry.AdditionalInfo,
                Type = moduleEntry.Type,
                TextLecture = moduleEntry.TextLecture?.Select(x => x.ToDto()).ToArray(),
                VideoLecture = moduleEntry.VideoLecture?.Select(x => x.ToDto()).ToArray(),
            };

        public static ModuleEntryListDto.ModuleEntryListItemDto ToListItemDto(this ModuleEntry moduleEntry) =>
            new ModuleEntryListDto.ModuleEntryListItemDto
            {
                Id = moduleEntry.Id,
                Name = moduleEntry.Name,
                IndexNumber = moduleEntry.IndexNumber,
                Description = moduleEntry.Description,
                AdditionalInfo = moduleEntry.AdditionalInfo,
                Type = moduleEntry.Type,
            };

        public static ModuleDto ToDto(this Module module) =>
            new ModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                IndexNumber = module.IndexNumber,
                Description = module.Description,
                Entries = module.Entries?.Select(x => x.ToDto()).ToArray(),
            };

        public static ModuleListDto.ModuleListItemDto ToListItemDto(this Module module) =>
            new ModuleListDto.ModuleListItemDto
            {
                Id = module.Id,
                Name = module.Name,
                IndexNumber = module.IndexNumber,
                Description = module.Description,
            };
        
        public static PhotoDto ToDto(this Photo photo) =>
            new PhotoDto
            {
                Id = photo.Id,
                FileUrl = photo.FileUrl,
                ThumbnailUrl = photo.ThumbnailUrl,
            };

        public static UserDto ToDto(this User user) =>
            new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                IsAdmin = user.IsAdmin,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RegistrationDate = user.RegistrationDate,
                ResidenceCity = user.ResidenceCity?.ToDto(),
            };
    }
}