using StudentDataImporter.Api.Configuration;
using StudentDataImporter.Api.DataAccess.Entities;
using StudentDataImporter.Api.DataAccess.Repositories;
using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public class DataImporter: IDataImporter
{
    private readonly ILogger<DataImporter> _logger;
    private readonly IRowParser _rowParser;
    private readonly ConfigurationConstants _constants;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<School> _schoolRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<CourseEnrollment> _courseEnrollmentRepository;
    private readonly IRepository<SchoolEnrollment> _schoolEnrollmentRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<CourseGrade> _courseGradeRepository;

    public DataImporter(ILogger<DataImporter> logger,
        IRowParser rowParser,
        ConfigurationConstants constants,
        IRepository<District> districtRepository,
        IRepository<School> schoolRepository,
        IRepository<Course> courseRepository,
        IRepository<CourseEnrollment> courseEnrollmentRepository,
        IRepository<SchoolEnrollment> schoolEnrollmentRepository,
        IRepository<Student> studentRepository,
        IRepository<CourseGrade> courseGradeRepository)
    {
        _logger = logger;
        _rowParser = rowParser;
        _constants = constants;
        _districtRepository = districtRepository;
        _schoolRepository = schoolRepository;
        _courseRepository = courseRepository;
        _courseEnrollmentRepository = courseEnrollmentRepository;
        _schoolEnrollmentRepository = schoolEnrollmentRepository;
        _studentRepository = studentRepository;
        _courseGradeRepository = courseGradeRepository;
    }
    
    public async Task<DataImporterResponse> ImportData(Stream data)
    {
        var reader = new StreamReader(data);
        await reader.ReadLineAsync();
        var batchSize = _constants.ImportBatchSize;

        var response = new DataImporterResponse();
        
        while (!reader.EndOfStream)
        {
            var rows = (await GetBatchOfRowsAsync(reader, batchSize)).ToList();

            var districtsImported = await _districtRepository.BulkInsertOrUpdateAsync(rows.Select(r => r.District).ToList());
            response.DistrictsImported += districtsImported;
            
            foreach (var row in rows)
            {
                row.School.DistrictId = row.District.Id;
            }
            var schoolsImported = await _schoolRepository.BulkInsertOrUpdateAsync(rows.Select(r => r.School).ToList());
            response.SchoolsImported += schoolsImported;
            
            foreach (var row in rows)
            {
                row.Course.SchoolId = row.School.Id;
            }
            var coursesImported = await _courseRepository.BulkInsertOrUpdateAsync(rows.Select(r => r.Course).ToList());
            response.CoursesImported += coursesImported;

            var studentsImported = await _studentRepository.BulkInsertOrUpdateAsync(rows.Select(row => row.Student).ToList());
            response.StudentsImported += studentsImported;
            
            var courseEnrollments = rows.Select(row => new CourseEnrollment
                { CourseId = row.Course.Id, StudentId = row.Student.Id });
            await _courseEnrollmentRepository.BulkInsertOrUpdateAsync(courseEnrollments.ToList());
            
            var schoolEnrollments = rows.Select(row => new SchoolEnrollment
                { SchoolId = row.School.Id, StudentId = row.Student.Id });
            await _schoolEnrollmentRepository.BulkInsertOrUpdateAsync(schoolEnrollments.ToList());
            
            var courseGrades = rows.Select(row => new CourseGrade
                { CourseId = row.Course.Id, StudentId = row.Student.Id, LetterGrade = row.Grade.LetterValue});
            var courseGradesImported = await _courseGradeRepository.BulkInsertOrUpdateAsync(courseGrades.ToList());
            response.CourseGradesImported += courseGradesImported;
        }

        return response;
    }

    private async Task<IEnumerable<Row>> GetBatchOfRowsAsync(StreamReader reader, int batchSize)
    {
        var rows = new List<Row>();
        for (var i = 0; i < batchSize && !reader.EndOfStream; i++)
        {
            var line = await reader.ReadLineAsync() ?? string.Empty;
            rows.Add(_rowParser.ParseRow(line));
        }

        return rows;
    }
}