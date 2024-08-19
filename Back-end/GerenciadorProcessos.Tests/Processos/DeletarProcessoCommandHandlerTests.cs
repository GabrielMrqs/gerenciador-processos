using FluentAssertions;
using GerenciadorProcessos.Application.CommandHandlers.Processos;
using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using Moq;

namespace GerenciadorProcessos.Tests.Processos
{
    public class DeletarProcessoCommandHandlerTests
    {
        private readonly Mock<IProcessoRepository> _repositoryMock;
        private readonly DeletarProcessoCommandHandler _handler;

        public DeletarProcessoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProcessoRepository>();
            _handler = new DeletarProcessoCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task HandleDeveRetornarExcecaoQuandoProcessoNaoExiste()
        {
            // Arrange
            var command = new DeletarProcessoCommand(Guid.NewGuid());
            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync((Processo)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>();
            result.Failure.Message.Should().Be("O processo não existe.");
        }

        [Fact]
        public async Task HandleDeveDeletarProcessoQuandoProcessoExiste()
        {
            // Arrange
            var command = new DeletarProcessoCommand(Guid.NewGuid());
            var processo = new Processo();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processo);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repositoryMock.Verify(repo => repo.Remove(processo), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task HandleDeveRetornarExcecaoQuandoRepositorioLancaExcecao()
        {
            // Arrange
            var command = new DeletarProcessoCommand(Guid.NewGuid());
            var processo = new Processo();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processo);
            _repositoryMock.Setup(repo => repo.Remove(processo))
                           .Throws(new Exception("Erro ao deletar processo"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>();
            result.Failure.Message.Should().Be("Erro ao deletar processo");
        }
    }
}