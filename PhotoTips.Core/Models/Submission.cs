using System;

namespace PhotoTips.Core.Models
{
    public class Submission
    {
        public enum SubmissionStatus
        {
            Checking,
            Passed,
            Rejected
        }
        
        public string Id { get; set; }
        public User Submitter { get; set; }
        public ModuleEntry ModuleEntry { get; set; }
        public Photo Photo { get; set; }
        public SubmissionStatus Status { get; set; }
        public DateTime Time { get; set; }
    }
}