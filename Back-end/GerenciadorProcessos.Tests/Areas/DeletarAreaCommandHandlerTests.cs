using FluentAssertions;
using GerenciadorProcessos.Application.CommandHandlers.Areas;
using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using MediatR;
using Moq;

namespace GerenciadorProcessos.Tests.Areas
{
    public class DeletarAreaCommandHandlerTests
    {
        private readonly Mock<IAreaRepository> _repositoryMock;
        private readonly DeletarAreaCommandHandler _handler;

        public DeletarAreaCommandHandlerTests()
        {
            _repositoryMock = new Mock<IAreaRepository>();
            _handler = new DeletarAreaCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_AreaNaoExiste_DeveRetornarException()
        {
            // Arrange
            var command = new DeletarAreaCommand(Guid.NewGuid());
            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync((Area)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("A área não existe.");
        }

        [Fact]
        public async Task Handle_AreaValida_DeveRemoverAreaComSucesso()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Nome = "Área Teste" };
            var command = new DeletarAreaCommand(area.Id);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(area);

            _repositoryMock.Setup(repo => repo.Remove(It.IsAny<Area>()));
            _repositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.Should().Be(Unit.Value);
            _repositoryMock.Verify(repo => repo.Remove(It.IsAny<Area>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ErroAoRemoverArea_DeveRetornarException()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Nome = "Área Teste" };
            var command = new DeletarAreaCommand(area.Id);

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(area);

            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("Erro ao remover"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Erro ao remover");
        }
    }
}
