﻿using FladeUp_API.Models.User;
using System.Drawing;

namespace FladeUp_API.Models.Subject
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public UserPublicDataModel Teacher { get; set; }
    }

    public class SubjectsResult
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalSubjects { get; set; }
        public List<SubjectModel> Subjects { get; set; }
    }
}
