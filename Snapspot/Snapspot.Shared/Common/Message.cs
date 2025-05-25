using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Shared.Common
{
    public partial class Message
    {
        // Success messages
        public const string I0000 = "The operation was successful!"; 
        public const string I0001 = "The item was added successfully.";
        public const string I0002 = "The item was updated successfully."; 

        // Warning messages
        public const string W0000 = "Do you want to proceed with this action?"; 
        public const string W0001 = "This action may have unintended consequences."; 
        public const string W0002 = "You are about to overwrite existing data. Are you sure?";

        // Error messages
        public const string E0000 = "The operation wasn't successful!";
        public const string E0001 = "The item could not be found.";
        public const string E0002 = "An unexpected error occurred. Please try again later."; 

        // Mapping ID to message
        private static readonly Dictionary<string, string> _messages = new()
        {
            { nameof(I0000), I0000 },
            { nameof(I0001), I0001 },
            { nameof(I0002), I0002 },
            { nameof(W0000), W0000 },
            { nameof(W0001), W0001 },
            { nameof(W0002), W0002 },
            { nameof(E0000), E0000 },
            { nameof(E0001), E0001 },
            { nameof(E0002), E0002 }
        };

        // Method to get message content by ID
        public static string GetMessageById(string id)
        {
            return _messages.TryGetValue(id, out var message) ? message : "Unknown message.";
        }
    }
}
