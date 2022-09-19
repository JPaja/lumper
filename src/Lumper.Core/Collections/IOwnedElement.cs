//Cloned from AsmResolver.Collections under MIT License
//https://github.com/Washi1337/AsmResolver/blob/master/src/AsmResolver/Collections/IOwnedCollectionElement.cs
//
namespace Lumper.Core.Collections;

/// <summary>
/// Represents an element in a collection or dictionary owned by a single object.
/// </summary>
/// <typeparam name="TOwner">The type of the object that owns the collection or dictionary.</typeparam>
public interface IOwnedElement<TOwner>
{
    /// <summary>
    /// Gets or sets the owner of the collection or dictionary.
    /// </summary>
    /// <remarks>
    /// This property should not be assigned directly.
    /// </remarks>
    TOwner? Owner
    {
        get;
        internal set;
    }
}
