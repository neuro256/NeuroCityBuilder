using UnityEngine;

namespace NeuroCityBuilder.Application.Interfaces
{
    public interface IGridCell
    {
        void SetDefault();
        void SetHighlightValid();
        void SetHighlightInvalid();
    }
}

