namespace DM.MovieApi.IntegrationTests.MovieDb.Configuration;

[TestClass]
public class ApiConfigurationRequestTests
{
    private IApiConfigurationRequest _api;

    [TestInitialize]
    public void TestInit()
    {
        ApiResponseUtil.ThrottleTests();

        _api = MovieDbFactory.Create<IApiConfigurationRequest>().Value;

        Assert.IsInstanceOfType( _api, typeof( ApiConfigurationRequest ) );
    }

    [TestMethod]
    public async Task ApiConfiguration_Includes_ListOf_ChangedKeys()
    {
        ApiQueryResponse<ApiConfiguration> config = await _api.GetAsync();

        Assert.IsNotNull( config.Item.ChangeKeys );
        Assert.IsTrue( config.Item.ChangeKeys.Any() );
    }

    [TestMethod]
    public async Task Images_RootUrl_SameAs_SecureRootUrl_WithHTTPS()
    {
        ApiQueryResponse<ApiConfiguration> config = await _api.GetAsync();

        string expectedSecureUrl = config.Item.Images.RootUrl.Replace( "http://", "https://" );

        Assert.AreEqual( expectedSecureUrl, config.Item.Images.SecureRootUrl );
    }

    [TestMethod]
    public async Task Images_RootUrls_AreStatic()
    {
        const string expectedUrl = "http://image.tmdb.org/t/p/";
        const string expectedSecureUrl = "https://image.tmdb.org/t/p/";

        ApiQueryResponse<ApiConfiguration> config = await _api.GetAsync();

        Assert.AreEqual( expectedUrl, config.Item.Images.RootUrl );
        Assert.AreEqual( expectedSecureUrl, config.Item.Images.SecureRootUrl );
    }

    [TestMethod]
    public async Task Images_Collections_Have_AtLeast_ThreeItems()
    {
        const int minCount = 3;

        ApiQueryResponse<ApiConfiguration> config = await _api.GetAsync();

        Assert.IsTrue( config.Item.Images.BackDrops.Count >= minCount );
        Assert.IsTrue( config.Item.Images.Logos.Count >= minCount );
        Assert.IsTrue( config.Item.Images.Posters.Count >= minCount );
        Assert.IsTrue( config.Item.Images.Profiles.Count >= minCount );
        Assert.IsTrue( config.Item.Images.Stills.Count >= minCount );
    }
}
