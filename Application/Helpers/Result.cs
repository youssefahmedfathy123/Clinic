﻿
namespace Application.Helpers
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }

        public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };
        public static Result<T> Fail(string error) => new() { Succeeded = false, Error = error };
    }
}

