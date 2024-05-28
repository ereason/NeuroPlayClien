using System.Collections.Generic;

namespace AppSharedClasses.Models
{
    public class ServiceResult {
        public bool Success { get; set; }
        public bool Fail => !Success;
        public List<string> Errors { get; set; }

        public ServiceResult(bool success) {
            Success = success;
            Errors = new List<string>();
        }

        public ServiceResult(bool success, List<string> errors) {
            Success = success;
            Errors = errors;
        }

        public ServiceResult() { }
    }

    public class ServiceResult<T> : ServiceResult where T : class{
        public T Data { get; set; }
        public ServiceResult(bool success) {
            Success = success;
            Data = null;
            Errors = new List<string>();
        }

        public ServiceResult(bool success, T data) {
            Success = success;
            Data = data;
            Errors = new List<string>();
        }

        public ServiceResult(bool success, List<string> errors) {
            Success = success;
            Errors = errors;
            Data = null;
        }
    }
}