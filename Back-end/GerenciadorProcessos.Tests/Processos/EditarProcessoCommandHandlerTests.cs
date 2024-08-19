using FluentAssertions;
using GerenciadorProcessos.Application.CommandHandlers.Processos;
using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Enums;
using GerenciadorProcessos.Domain.Repositorios;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace GerenciadorProcessos.Tests.Processos
{
    public class EditarProcessoCommandHandlerTests
    {
        private readonly Mock<IProcessoRepository> _repositoryMock;
        private readonly EditarProcessoCommandHandler _handler;

        public EditarProcessoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProcessoRepository>();
            _handler = new EditarProcessoCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ProcessoNaoExiste_DeveRetornarException()
        {
            // Arrange
            var command = new EditarProcessoCommand { Id = Guid.NewGuid(), Nome = "Processo Teste" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync((Processo)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be($"Processo {command.Id} não existe.");
        }

        [Fact]
        public async Task Handle_ProcessoComMesmoNomeEOutroId_DeveRetornarException()
        {
            // Arrange
            var processoExistente = new Processo { Id = Guid.NewGuid(), Nome = "Processo Teste" };
            var command = new EditarProcessoCommand { Id = Guid.NewGuid(), Nome = "Processo Teste" };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processoExistente);

            Expression<Func<Processo, bool>> nomeIgualEIdDiferente = a => a.Nome.ToUpper().Equals(command.Nome.ToUpper()) && a.Id != command.Id;
            _repositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Processo, bool>>>()))
                               .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be($"Processo '{command.Nome}' já existente.");
        }

        [Fact]
        public async Task Handle_TentarVincularProcessoPaiComoSubprocesso_DeveRetornarException()
        {
            // Arrange
            var processo = new Processo { Id = Guid.NewGuid(), Nome = "Processo Teste", ProcessoPai = new Processo() };
            var command = new EditarProcessoCommand
            {
                Id = processo.Id,
                Nome = "Processo Teste",
                Subprocessos = new List<Guid> { processo.ProcessoPai.Id }
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync(processo);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Não é possível vincular um processo pai como subprocesso.");
        }

        [Fact]
        public async Task Handle_SubprocessosJaAtrelados_DeveRetornarException()
        {
            // Arrange
            var processo = new Processo { Id = Guid.NewGuid(), Nome = "Processo Teste" };
            var subprocesso = new Processo { Id = Guid.NewGuid(), ProcessoPaiId = Guid.NewGuid() };
            var command = new EditarProcessoCommand
            {
                Id = processo.Id,
                Nome = "Processo Teste",
                Subprocessos = new List<Guid> { subprocesso.Id }
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processo);

            _repositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                           .ReturnsAsync(new List<Processo> { subprocesso });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Um ou mais processos especificados já estão atrelados a outros processos.");
        }

        [Fact]
        public async Task Handle_SubprocessosNaoExistem_DeveRetornarException()
        {
            // Arrange
            var processo = new Processo { Id = Guid.NewGuid(), Nome = "Processo Teste" };
            var command = new EditarProcessoCommand
            {
                Id = processo.Id,
                Nome = "Processo Teste",
                Subprocessos = new List<Guid> { Guid.NewGuid() }
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processo);

            _repositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                           .ReturnsAsync(new List<Processo>()); // Nenhum subprocesso encontrado

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Um ou mais subprocessos especificados não existem.");
        }

        [Fact]
        public async Task Handle_ProcessoValido_DeveAtualizarProcessoComSucesso()
        {
            // Arrange
            var processo = new Processo { Id = Guid.NewGuid(), Nome = "Processo Teste" };
            var command = new EditarProcessoCommand
            {
                Id = processo.Id,
                Nome = "Processo Atualizado",
                Descricao = "Descrição Atualizada",
                Tipo = TipoProcesso.Manual
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processo);

            _repositoryMock.Setup(repo => repo.Update(It.IsAny<Processo>()));
            _repositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.Should().Be(Unit.Value);
            _repositoryMock.Verify(repo => repo.Update(It.IsAny<Processo>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ErroAoAtualizarProcesso_DeveRetornarException()
        {
            // Arrange
            var processo = new Processo { Id = Guid.NewGuid(), Nome = "Processo Teste" };
            var command = new EditarProcessoCommand
            {
                Id = processo.Id,
                Nome = "Processo Atualizado"
            };

            _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                           .ReturnsAsync(processo);

            _repositoryMock.Setup(repo => repo.SaveChangesAsync())
                           .ThrowsAsync(new Exception("Erro ao atualizar"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Erro ao atualizar");
        }
    }
}
