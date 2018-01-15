using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.XA.Foundation.Theming.Bundler;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Theming.Configuration;
using Sitecore.XA.Foundation.Theming;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Configuration;
using Sitecore.Pipelines;
using Sitecore.SecurityModel.License;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.SitecoreExtensions.Comparers;
using Sitecore.XA.Foundation.SitecoreExtensions.Repositories;
using Sitecore.XA.Foundation.Theming.EventHandlers;
using Sitecore.XA.Foundation.Theming.Extensions;
using Sitecore.XA.Foundation.Theming.Pipelines.AssetService;

namespace Sitecore.Support.XA.Foundation.Theming.Bundler
{

  public class AssetLinksGenerator: Sitecore.XA.Foundation.Theming.Bundler.AssetLinksGenerator
  {

    public static new AssetLinks GenerateLinks(IThemesProvider themesProvider)
    {
      if (AssetContentRefresher.IsPublishing() || IsAddingRendering())
      {
        return new AssetLinks();
      }

      AssetLinksGenerator linksGenerator = new AssetLinksGenerator();
      return linksGenerator.GenerateAssetLinks(themesProvider);
    }
    protected override string GetOptimizedItemLink(Item theme, OptimizationType type, AssetServiceMode mode, string query, string fileName)
    {
      query = string.Format(query, Templates.OptimizedFile.ID, fileName);
      Item optimizedScriptItem = theme.Axes.SelectSingleItem(query);
      if (optimizedScriptItem != null && IsNotEmpty(optimizedScriptItem))
      {
        return optimizedScriptItem.BuildAssetPath(true);
      }
      return (new AssetBundler()).GetOptimizedItemPath(theme, type, mode);
    }

    private bool IsNotEmpty(Item optimizedScriptItem)
    {
      MediaItem scriptItem = optimizedScriptItem;
      using (var mediaStream = scriptItem.GetMediaStream())
      {
        return mediaStream != null && mediaStream.Length > 0;
      }
    }
    
  }
}