using SYOS.Shared.DTO;

namespace SYOS.Shared.Interfaces
{
    public interface IShelfService
    {
        /// <summary>
        /// Retrieves all shelves.
        /// </summary>
        /// <returns>A list of all ShelfDTO objects.</returns>
        Task<List<ShelfDTO>> GetAllShelvesAsync();

        /// <summary>
        /// Retrieves a specific shelf by its ID.
        /// </summary>
        /// <param name="shelfId">The ID of the shelf to retrieve.</param>
        /// <returns>The ShelfDTO object if found, null otherwise.</returns>
        Task<ShelfDTO> GetShelfByIdAsync(int shelfId);

        /// <summary>
        /// Adds a new shelf.
        /// </summary>
        /// <param name="shelf">The ShelfDTO object representing the new shelf.</param>
        /// <returns>The added ShelfDTO with updated information (e.g., assigned ID).</returns>
        Task<ShelfDTO> AddShelfAsync(ShelfDTO shelf);

        /// <summary>
        /// Updates an existing shelf.
        /// </summary>
        /// <param name="shelf">The ShelfDTO object with updated information.</param>
        /// <returns>The updated ShelfDTO.</returns>
        Task<ShelfDTO> UpdateShelfAsync(ShelfDTO shelf);

        /// <summary>
        /// Deletes a shelf by its ID.
        /// </summary>
        /// <param name="shelfId">The ID of the shelf to delete.</param>
        Task DeleteShelfAsync(int shelfId);

        /// <summary>
        /// Assigns items to a shelf.
        /// </summary>
        /// <param name="shelfId">The ID of the shelf to assign items to.</param>
        /// <param name="itemCode">The code of the item to assign.</param>
        /// <param name="quantity">The quantity of items to assign.</param>
        Task AssignItemsToShelfAsync(int shelfId, string itemCode, int quantity);

        /// <summary>
        /// Retrieves shelves by item code.
        /// </summary>
        /// <param name="itemCode">The code of the item to search for.</param>
        /// <returns>A list of ShelfDTO objects containing the specified item.</returns>
        Task<List<ShelfDTO>> GetShelvesByItemCodeAsync(string itemCode);
    }
}