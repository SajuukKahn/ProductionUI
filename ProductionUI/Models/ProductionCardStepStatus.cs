using System.Drawing;

namespace ProductionUI.Models
{
    public class ProductionCardStepStatus
    {
        protected readonly uint Status;
        protected readonly string StatusReadout;

        public static readonly ProductionCardStepStatus Waiting = new ProductionCardStepStatus(0, nameof(Waiting));
        public static readonly ProductionCardStepStatus Running = new ProductionCardStepStatus(1, nameof(Running));
        public static readonly ProductionCardStepStatus Pausing = new ProductionCardStepStatus(2, nameof(Pausing));
        public static readonly ProductionCardStepStatus Paused = new ProductionCardStepStatus(3, nameof(Paused));
        public static readonly ProductionCardStepStatus Completed = new ProductionCardStepStatus(4, nameof(Completed));

        protected ProductionCardStepStatus(uint status, string statusReadout)
        {
            Status = status;
            StatusReadout = statusReadout;
        }

        public override string ToString()
        {
            return StatusReadout;
        }

        public static implicit operator uint(ProductionCardStepStatus @enum)
        {
            return @enum.Status;
        }

        public static implicit operator string(ProductionCardStepStatus @enum)
        {
            return @enum.StatusReadout;
        }
    }
}
