using NUnit.Framework;
using NetGuard_GT.Models;

namespace NetGuardGT.Tests
{
    public class IncidenteTests
    {
        [Test]
        public void Incidente_DebeCrearseConEstadoRegistrado()
        {
            var incidente = new Incidente
            {
                Titulo = "Corte de fibra",
                Descripcion = "Falla en enlace principal",
                TipoIncidente = "Fibra Optica",
                Severidad = "Alta",
                SitioRedId = 1
            };

            Assert.That(incidente.Estado, Is.EqualTo("Registrado"));
        }

        [Test]
        public void Tecnico_DebeEstarActivoPorDefecto()
        {
            var tecnico = new Tecnico
            {
                Nombre = "Carlos Méndez",
                Especialidad = "Fibra Optica"
            };

            Assert.That(tecnico.Activo, Is.True);
        }

        [Test]
        public void Incidente_NoDebeEstarEscaladoPorDefecto()
        {
            var incidente = new Incidente();

            Assert.That(incidente.Escalado, Is.False);
        }
    }
}