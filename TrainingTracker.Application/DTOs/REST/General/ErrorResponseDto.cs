﻿namespace TrainingTracker.Application.DTOs.REST.General
{
    public class ErrorResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public int StatusCode { get; set; }
    }
}
