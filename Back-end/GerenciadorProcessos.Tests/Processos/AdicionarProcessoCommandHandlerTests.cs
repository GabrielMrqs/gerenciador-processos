using AutoMapper;
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

    public class AdicionarProcessoCommandHandlerTests
    {
        private readonly Mock<IProcessoRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdicionarProcessoCommandHandler _handler;

        public AdicionarProcessoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProcessoRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AdicionarProcessoCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ProcessoJaExistente_DeveRetornarException()
        {
            // Arrange
            var command = new AdicionarProcessoCommand { Nome = "Processo Teste" };
            _repositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Processo, bool>>>()))
                           .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Processo 'Processo Teste' já existente.");
        }

        [Fact]
        public async Task Handle_SubprocessosJaAtrelados_DeveRetornarException()
        {
            // Arrange
            var command = new AdicionarProcessoCommand
            {
                Nome = "Processo Teste",
                Subprocessos = new List<Guid> { Guid.NewGuid() }
            };

            var subprocesso = new Processo { Id = command.Subprocessos.First(), ProcessoPaiId = Guid.NewGuid() };

            _repositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Processo, bool>>>()))
                           .ReturnsAsync(false);

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
            var command = new AdicionarProcessoCommand
            {
                Nome = "Processo Teste",
                Subprocessos = new List<Guid> { Guid.NewGuid() }
            };

            _repositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Processo, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                           .ReturnsAsync(new List<Processo>()); // Nenhum subprocesso encontrado

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Um ou mais subprocessos especificados não existem.");
        }

        [Fact]
        public async Task Handle_ProcessoValido_DeveAdicionarProcessoComSucesso()
        {
            // Arrange
            var command = new AdicionarProcessoCommand
            {
                Nome = "Processo Teste",
                Descricao = "Descrição Teste",
                Tipo = TipoProcesso.Manual
            };

            var processo = new Processo();

            _repositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Processo, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Processo>()))
                           .Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<Processo>(It.IsAny<AdicionarProcessoCommand>()))
                       .Returns(processo);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.Should().Be(Unit.Value);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Processo>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ErroAoSalvarProcesso_DeveRetornarException()
        {
            // Arrange
            var command = new AdicionarProcessoCommand { Nome = "Processo Teste" };
            var processo = new Processo();

            _repositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Processo, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Processo>()))
                           .ThrowsAsync(new Exception("Erro ao salvar"));

            _mapperMock.Setup(m => m.Map<Processo>(It.IsAny<AdicionarProcessoCommand>()))
                       .Returns(processo);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Erro ao salvar");
        }
    }

}