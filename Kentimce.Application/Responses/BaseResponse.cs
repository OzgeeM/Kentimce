using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.Responses
{
    public class BaseResponse<T> //: IStatusCodeActionResult, IActionResult
    {

        public T data { get; set; }
        public String[] error { get; set; }
        public int status { get; set; }

        public bool IsSuccess => status >= 200 && status < 299;

        public BaseResponse<T> Success(T data)
        {
            this.SetData(data);
            this.SetStatus(200);
            return this;
        }
        public BaseResponse<T> NoContent(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
            {
                this.SetError(new string[1] { message });
            }
            this.SetStatus(204);
            return this;
        }

        public BaseResponse<T> Success(T data, int status)
        {

            this.SetStatus(status);
            return this;
        }

        public BaseResponse<T> Fail(string err)
        {

            this.SetError(new string[1] { err });
            this.SetStatus(500);
            return this;
        }

        public BaseResponse<T> Fail(string[] err)
        {

            this.SetError(err);
            this.SetStatus(500);
            return this;
        }

        public BaseResponse<T> Fail(string[] err, int status)
        {

            this.SetError(err);
            this.SetStatus(status);
            return this;
        }

        public virtual T GetData()
        {
            return this.data;
        }

        public virtual void SetData(T _data)
        {
            this.data = _data;
        }

        public virtual String[] GetError()
        {
            return this.error;
        }

        public virtual void SetError(String[] _error)
        {
            this.error = _error;
        }

        public virtual int GetStatus()
        {
            return this.status;
        }

        public virtual void SetStatus(int _status)
        {
            this.status = _status;
        }
    }
}
