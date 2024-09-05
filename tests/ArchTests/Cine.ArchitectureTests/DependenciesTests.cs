using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using System.Text.Json;

namespace Cine.ArchitectureTests
{
    public class DependenciesTests
    {
        public record ProjectInfo
        {
            public required string Name { get; set; }

            public required string Path { get; set; }
        }

        public class ProjectInfos
        {
            public required List<ProjectInfo> Projects { get; set; }
        }

        public static class ProjectsLoader
        {
            public static ProjectInfo[] LoadProjects(string filePath)
            {
                var json = File.ReadAllText(filePath)!;

                return [.. JsonSerializer.Deserialize<ProjectInfos>(json)!.Projects];
            }
        }

        public static class AssemblyLoader
        {
            public static Assembly[] LoadAssemblies(IEnumerable<ProjectInfo> projectInfos)
            {
                return [.. projectInfos.Select(projectInfo => Assembly.LoadFrom(projectInfo.Path))];
            }

            public static Assembly[] LoadAssemblies(IEnumerable<ProjectInfo> projectInfos, string namePattern)
            {
                return LoadAssemblies(projectInfos.Where(projectInfo => projectInfo.Name.Contains(namePattern)));
            }
        }

        [Fact]
        public void DomainLayer_ShouldNotDependOnApplicationOrInfrastructure()
        {
            var projects = ProjectsLoader.LoadProjects("projects.json");

            var domainAssemblies = AssemblyLoader.LoadAssemblies(projects, "Domain");
            var nonDependentProjects = projects.Where(projectInfo => projectInfo.Name.ContainsAny(["Application", "Infrastructure"])).Select(projectInfo => projectInfo.Name).ToArray();

            foreach (var domain in domainAssemblies)
            {
                var result = Types.InAssembly(domain)
                     .Should().NotHaveDependencyOnAny(nonDependentProjects)
                     .GetResult();

                result.IsSuccessful.Should().BeTrue();
            }
        }

        [Fact]
        public void ApplicationLayer_ShouldNotDependOnInfrastructure()
        {
            var projects = ProjectsLoader.LoadProjects("projects.json");

            var applicationAssemblies = AssemblyLoader.LoadAssemblies(projects, "Application");
            var nonDependentProjects = projects.Where(projectInfo => projectInfo.Name.Contains("Infrastructure")).Select(projectInfo => projectInfo.Name).ToArray();

            foreach (var application in applicationAssemblies)
            {
                var result = Types.InAssembly(application)
                     .Should().NotHaveDependencyOnAny(nonDependentProjects)
                     .GetResult();

                result.IsSuccessful.Should().BeTrue();
            }
        }

        [Fact]
        public void InfrastructureLayer_ShouldNotDependOnApi()
        {
            var projects = ProjectsLoader.LoadProjects("projects.json");

            var infrastructureAssemblies = AssemblyLoader.LoadAssemblies(projects, "Infrastructure");
            var nonDependentProjects = projects.Where(projectInfo => projectInfo.Name.Contains("Api")).Select(projectInfo => projectInfo.Name).ToArray();

            foreach (var infrastructure in infrastructureAssemblies)
            {
                var result = Types.InAssembly(infrastructure)
                     .Should().NotHaveDependencyOnAny(nonDependentProjects)
                     .GetResult();

                result.IsSuccessful.Should().BeTrue();
            }
        }
    }
}
