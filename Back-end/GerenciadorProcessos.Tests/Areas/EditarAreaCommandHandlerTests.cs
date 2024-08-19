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
    public class EditarAreaCommandHandlerTests
    {
        private readonly Mock<IAreaRepository> _areaRepositoryMock;
        private readonly Mock<IProcessoRepository> _processoRepositoryMock;
        private readonly EditarAreaCommandHandler _handler;

        public EditarAreaCommandHandlerTests()
        {
            _areaRepositoryMock = new Mock<IAreaRepository>();
            _processoRepositoryMock = new Mock<IProcessoRepository>();
            _handler = new EditarAreaCommandHandler(_areaRepositoryMock.Object, _processoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_AreaNaoExiste_DeveRetornarException()
        {
            // Arrange
            var command = new EditarAreaCommand { Id = Guid.NewGuid(), Nome = "Área Atualizada" };
            _areaRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                               .ReturnsAsync((Area)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be($"Área {command.Id} não existe.");
        }

        [Fact]
        public async Task Handle_AreaComMesmoNomeEOutroId_DeveRetornarException()
        {
            // Arrange
            var areaExistente = new Area { Id = Guid.NewGuid(), Nome = "Área Existente" };
            var command = new EditarAreaCommand { Id = Guid.NewGuid(), Nome = "Área Existente" };

            _areaRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                               .ReturnsAsync(new Area { Id = command.Id });

            _areaRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Area, bool>>>()))
                   .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be($"Área '{command.Nome}' já existente.");
        }

        [Fact]
        public async Task Handle_ProcessosJaAtrelados_DeveRetornarException()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Nome = "Área Atualizada" };
            var subprocesso = new Processo { Id = Guid.NewGuid(), AreaId = Guid.NewGuid() };
            var command = new EditarAreaCommand
            {
                Id = area.Id,
                Nome = "Área Atualizada",
                Processos = new List<Guid> { subprocesso.Id }
            };

            _areaRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                               .ReturnsAsync(area);

            _processoRepositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                                   .ReturnsAsync(new List<Processo> { subprocesso });

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
            var area = new Area { Id = Guid.NewGuid(), Nome = "Área Atualizada" };
            var command = new EditarAreaCommand
            {
                Id = area.Id,
                Nome = "Área Atualizada",
                Processos = new List<Guid> { Guid.NewGuid() }
            };

            _areaRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                               .ReturnsAsync(area);

            _processoRepositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                                   .ReturnsAsync(new List<Processo>()); // Nenhum processo encontrado

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Um ou mais processos especificados não existem.");
        }

        [Fact]
        public async Task Handle_AreaValida_DeveAtualizarAreaComSucesso()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Nome = "Área Atualizada" };
            var command = new EditarAreaCommand
            {
                Id = area.Id,
                Nome = "Área Atualizada",
                Descricao = "Descrição Atualizada",
                Processos = new List<Guid> { Guid.NewGuid() }
            };

            _areaRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                               .ReturnsAsync(area);

            _processoRepositoryMock.Setup(repo => repo.FindAllByIds(It.IsAny<IEnumerable<Guid>>()))
                                   .ReturnsAsync(new List<Processo>() { new Processo()});

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Success.Should().Be(Unit.Value);
            _areaRepositoryMock.Verify(repo => repo.Update(It.IsAny<Area>()), Times.Once);
            _areaRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ErroAoAtualizarArea_DeveRetornarException()
        {
            // Arrange
            var area = new Area { Id = Guid.NewGuid(), Nome = "Área Atualizada" };
            var command = new EditarAreaCommand
            {
                Id = area.Id,
                Nome = "Área Atualizada"
            };

            _areaRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                               .ReturnsAsync(area);

            _areaRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                               .ThrowsAsync(new Exception("Erro ao atualizar"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failure.Should().BeOfType<Exception>().Which.Message.Should().Be("Erro ao atualizar");
        }
    }
}
