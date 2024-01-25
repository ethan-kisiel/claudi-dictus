using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

namespace DictusClaudi;

/*
<div class="card-body">
    {% if page_obj.has_previous %}
    <a class="card-link" href="?page={{ page_obj.previous_page_number }}">&lt;</a>
    {% endif %}

    {% if page_obj.paginator.num_pages >= 9 %}


        <a class="card-link {% if  page_obj.number == 1 %}text-decoration-underline{% endif %}" href="?page=1">1</a>

        {% for x in page_obj.number|get_pages_from_curr:page_obj.paginator.num_pages %}
        <a class="card-link {% if x == page_obj.number %}text-decoration-underline{% endif %}"
        href="?page={{ x }}">{{ x }}</a>
        {% endfor %}

        <a class="card-link {% if page_obj.number == page_obj.paginator.num_pages %}text-decoration-underline{% endif %}"
        href="?page={{ page_obj.paginator.num_pages }}">{{ page_obj.paginator.num_pages }}</a>

    {% else %}
        {% for x in page_obj.paginator.num_pages|get_pages_arr %}
            <a class="card-link {% if x == page_obj.number %}text-decoration-underline{% endif %}" href="?page={{ x }}">{{ x }}</a>
        {% endfor %}
    {% endif%}

    {% if page_obj.has_next %}
    <a class="card-link" href="?page={{ page_obj.next_page_number }}">&gt;</a>
    {% endif %}
</div>



## THIS STUFF IS FOR THE PAGINATION PICKER
@register.filter
def get_pages_arr(num_pages: int):
    """
    Takes an integer num_pages, returns generator which returns the index+1
    of the range 0 through num_pages.
    """
    for page_num in range(num_pages):
        yield page_num + 1


@register.filter
def get_pages_from_curr(curr_page: int, last_page: int):
    """
    Takes a current page number and a last page number as integers,
    returns an array of numbers, where the numbers represent the page
    selector
    NOTE: when this is used in a display, the first and last page numbers
    will be hard coded in, therefor, this function is really just for
    producing the 'creme of the oreo'.
    """

    num_pages = 7
    half_pages = int(num_pages / 2)

    if curr_page - half_pages < 1:
        # if the current page is closer to page one, return the
        # 2nd

        for x in range(2, (curr_page + ((num_pages + 1) - curr_page))):
            yield x

    elif (last_page - curr_page) < half_pages:
        # if the current page is closer to the last page display
        # starts at the current page number minus the difference between

        floor = curr_page - (num_pages - (last_page - curr_page))
        for x in range(floor, last_page):
            yield x

    else:
        # the +1 occurs due to the exclusive nature of the maximum bound of teh range function

        for x in range(curr_page - half_pages, (curr_page + half_pages) + 1):
            yield x

*/


struct PaginationResult<T>
{
    public bool IsPrimaryParameter;
    public int PageNumber;
    public int LastPageNumber;
    
    public int MaxTabs; // Maximum number of tabs to display in the selector

    public bool HasNextPage
    {
        get { return PageNumber < LastPageNumber; }
    }
    
    public bool HasPreviousPage
    {
        get { return PageNumber > 1; }
    }

    public string ParameterPrefix
    {
        get { return IsPrimaryParameter ? "?page=" : "&page="; }
    }

    public string ParameterizedHref(int pageNumber, string? letter, string[]? searchTerms)
    {
        string parameterQuery = "";
        if (!letter.IsNullOrEmpty())
        {
            parameterQuery = $"?selectedLetter={letter}&page={pageNumber}";
        } else if (!searchTerms.IsNullOrEmpty())
        {
            var joinedSearchTerms = string.Join('+', searchTerms);
            parameterQuery = $"?searchTerms={joinedSearchTerms}&page={pageNumber}";
        } else
        {
            parameterQuery = $"?page={pageNumber}";
        }

        return parameterQuery;
    }

    public int[] MiddlePageNumbers
    {
       get { 

        int[] pageNumbers;

        if (LastPageNumber == 0)
        {
            LastPageNumber = 1;
        }

        if (LastPageNumber < MaxTabs)
        {
            pageNumbers = new int[LastPageNumber];
            for (int i = 0; i < LastPageNumber; i++)
            {
                pageNumbers[i] = i+1;
            }
            return pageNumbers;
        }

        int numPages = MaxTabs-2;
        int halfPages = numPages / 2;

        int floor;
        int ceil;

        pageNumbers = new int[numPages];

        if (PageNumber - halfPages < 1)
        {
            floor = 2;
            ceil = numPages + 2;

        } else if ((LastPageNumber - PageNumber) < halfPages)
        {
            floor = PageNumber - (numPages - (LastPageNumber - PageNumber));
            ceil = LastPageNumber;
        } else
        {
            floor = PageNumber - halfPages;
            ceil = PageNumber + halfPages + 1;
            if (ceil > LastPageNumber)
            {
                ceil = LastPageNumber;
            }
        }

        int index = 0;

        for (int i = floor; i < ceil; i++)
        {
            pageNumbers[index++] = i;
        }

        return pageNumbers;
        }
    }
}

public static class PaginationUtility<T>
{
    public static int PageSize = 100;

    public static T[] GetPage(int page, List<T> values)
    {

        try
        {
            // the length of the slice will be the default page size if it fits within the limits of this item
            int startIndex = (page-1) * PageSize;
            int lastIndex = page * PageSize;
            int adjustedLastIndex = values.Count;

            int sliceLength = lastIndex >= values.Count ? adjustedLastIndex : PageSize;
            return new ArraySegment<T>(values.ToArray(), startIndex, sliceLength).ToArray();
        }
        catch (Exception e)
        {
            return new T[0];
        }
    }
}
