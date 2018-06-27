using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace VirtualSales.Core.Models
{
    public class MeetingStateModel : ReactiveObject
    {
        private ToolIdAndPageNumber? _state;

        public ToolIdAndPageNumber? State
        {
            get { return _state; }
            set
            {
                this.RaiseAndSetIfChanged(ref _state, value);
            }
        }
    }

    public struct ToolIdAndPageNumber : IEquatable<ToolIdAndPageNumber>
    {
        public bool Equals(ToolIdAndPageNumber other)
        {
            return ToolId.Equals(other.ToolId) && PageNumber == other.PageNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ToolIdAndPageNumber && Equals((ToolIdAndPageNumber)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ToolId.GetHashCode()*397) ^ PageNumber;
            }
        }

        public static bool operator ==(ToolIdAndPageNumber left, ToolIdAndPageNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ToolIdAndPageNumber left, ToolIdAndPageNumber right)
        {
            return !left.Equals(right);
        }

        public Guid ToolId { get; set; }
        public int PageNumber { get; set; }
    }

}
