using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class CardMapper
    {
        public CardMapper()
        {
            
        }

        public CardDto CreateDto(Card card)
        {
            return new CardDto
            {
                Id = card.Id,
                Title = card.Title,
                Description = card.Description,
                DueDate = card.DueDate
            };
        }

        public ICollection<CardDto> CreateDtoList(ICollection<Card> cards)
        {
            return cards.Select(CreateDto).ToList();
        }
    }
}
