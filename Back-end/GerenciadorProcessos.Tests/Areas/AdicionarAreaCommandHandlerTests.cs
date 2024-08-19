using AutoMapper;
using FluentAssertions;
using GerenciadorProcessos.Application.CommandHandlers.Areas;
using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using MediatR;
using Moq;
using System.Linq.Expressions;

namespace GerenciadorProcessos.Tests.Areas
{
    public class AdicionarAreaCommandHandlerTests
    {
        private readonly Mock<IAreaRepository> _areaRepositoryMock;
        private readonly Mock<IProcessoRepository> _processoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AdicionarAreaCommandHandler _handler;

        public AdicionarAreaCommandHandlerTests()
        {
            _areaRepositoryMock = new Mock<IAreaRepository>();
            _processoRepositoryMock = new Mock<IProcessoRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AdicionarAreaCommandHandler(_areaRepositoryMock.Object, _mapperMock.Object, _processoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_AreaJaExistente_DeveRetornarException()
        {
            // Arrange
            var command = new AdicionarAreaCommand { Nome = "Área Teste" };
            _areaRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Area, bool>>>()))
                               .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Área 'Área Teste' já existente.");
        }

        [Fact]
        public async Task Handle_ProcessosJaAtrelados_DeveRetornarException()
        {
            // Arrange
            var command = new AdicionarAreaCommand
            {
                Nome = "Área Teste",
                Processos = new List<Guid> { Guid.NewGuid() }
            };

            var processo = new Processo { Id = command.Processos.First(), AreaId = Guid.NewGuid() };

            _areaRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Area, bool>>>()))
                               .ReturnsAsync(false);

            _processoRepositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                                   .ReturnsAsync(new List<Processo> { processo });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Um ou mais processos especificados já estão atrelados a outras áreas.");
        }

        [Fact]
        public async Task Handle_ProcessosNaoExistem_DeveRetornarException()
        {
            // Arrange
            var command = new AdicionarAreaCommand
            {
                Nome = "Área Teste",
                Processos = new List<Guid> { Guid.NewGuid() }
            };

            _areaRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Area, bool>>>()))
                               .ReturnsAsync(false);

            _processoRepositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                                   .ReturnsAsync(new List<Processo>()); // Nenhum processo encontrado

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Um ou mais processos especificados não existem.");
        }

        [Fact]
        public async Task Handle_AreaValida_DeveAdicionarAreaComSucesso()
        {
            // Arrange
            var command = new AdicionarAreaCommand
            {
                Nome = "Área Teste",
                Processos = new List<Guid> { Guid.NewGuid() }
            };

            var area = new Area();

            _areaRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Area, bool>>>()))
                               .ReturnsAsync(false);

            _processoRepositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                                   .ReturnsAsync(new List<Processo>() { new Processo()});

            _mapperMock.Setup(m => m.Map<Area>(It.IsAny<AdicionarAreaCommand>()))
                       .Returns(area);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.Should().Be(Unit.Value);
            _areaRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Area>()), Times.Once);
            _areaRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}
