namespace DM.MovieApi.IntegrationTests.MovieDb.Companies;

[TestClass]
public class ApiCompanyRequestTests
{
    private IApiCompanyRequest _api;

    [TestInitialize]
    public void TestInit()
    {
        ApiResponseUtil.ThrottleTests();

        _api = MovieDbFactory.Create<IApiCompanyRequest>().Value;

        Assert.IsInstanceOfType( _api, typeof( ApiCompanyRequest ) );
    }

    [TestMethod]
    public async Task FindByIdAsync_Lucasfilm_WithResults_NoParentCompany()
    {
        const int id = 1;
        const string expectedName = "Lucasfilm";
        const string expectedHeadquarters = "San Francisco, California";
        const string expectedHomepage = "https://www.lucasfilm.com";

        ApiQueryResponse<ProductionCompany> response = await _api.FindByIdAsync( id );

        ApiResponseUtil.AssertErrorIsNull( response );

        Assert.AreEqual( id, response.Item.Id );
        Assert.AreEqual( expectedName, response.Item.Name );
        Assert.AreEqual( expectedHeadquarters, response.Item.Headquarters );
        Assert.AreEqual( new Uri( expectedHomepage ), new Uri( response.Item.Homepage ) );
        ApiResponseUtil.AssertImagePath( response.Item.LogoPath );

        Assert.IsNull( response.Item.ParentCompany );
    }

    [TestMethod]
    public async Task FindByIdAsync_Pixar_IncludesParentCompany()
    {
        const int id = 3;
        const string expectedName = "Pixar";
        const string expectedParentName = "Walt Disney Pictures";
        const int expectedParentId = 2;

        ApiQueryResponse<ProductionCompany> response = await _api.FindByIdAsync( id );

        ApiResponseUtil.AssertErrorIsNull( response );

        Assert.AreEqual( id, response.Item.Id );
        Assert.AreEqual( expectedName, response.Item.Name );

        Assert.IsNotNull( response.Item.ParentCompany );
        Assert.AreEqual( expectedParentId, response.Item.ParentCompany.Id );
        Assert.AreEqual( expectedParentName, response.Item.ParentCompany.Name );
        ApiResponseUtil.AssertImagePath( response.Item.ParentCompany.LogoPath );
    }

    [TestMethod]
    public async Task GetMoviesAsync_CanPageResults()
    {
        const int companyId = 3;
        const int minimumPageCount = 5;

        await ApiResponseUtil.AssertCanPageSearchResponse( companyId, minimumPageCount,
            ( id, pageNumber ) => _api.GetMoviesAsync( id, pageNumber ), x => x.Id );
    }
}
