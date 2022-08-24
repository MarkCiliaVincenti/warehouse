import SuiBox from "components/SuiBox";
import SuiTypography from "components/SuiTypography";
import Table from "examples/Tables/Table";
import { useQuery } from "react-query";
import { fetchObjects } from "utils/query-keys";
import { getObjects } from "services/administration-service";
import Card from "@mui/material/Card";
import { ProviderName } from "utils/providers";
import GppGoodIcon from "@mui/icons-material/GppGood";
import { useState } from "react";

const SecurityObjects = () => {
  const { isSuccess, data: response, isLoading, error } = useQuery([fetchObjects], getObjects);
  return (
    <Card>
      <SuiBox display="flex" justifyContent="space-between" alignItems="center" p={3}>
        <SuiTypography variant="h6">Objects</SuiTypography>
      </SuiBox>
      <SuiBox
        sx={{
          "& .MuiTableRow-root:not(:last-child)": {
            "& td": {
              borderBottom: ({ borders: { borderWidth, borderColor } }) =>
                `${borderWidth[1]} solid ${borderColor}`,
            },
          },
        }}
      >
        {isSuccess && (
          <Table
            columns={[
              { name: "name", align: "left" },
              { name: "description", align: "center" },
              { name: "provider", align: "center" },
            ]}
            rows={
              isSuccess &&
              response.items.map((item) => ({
                key: item.id,
                item: item,
                name: (
                  <SuiBox display="flex" alignItems="center" px={1} py={0.5}>
                    <SuiBox mr={2}>
                      <GppGoodIcon fontSize="large" />
                    </SuiBox>
                    <SuiBox display="flex" flexDirection="column">
                      <SuiTypography variant="button" fontWeight="medium">
                        {item.name ? item.name : "n/a"}
                      </SuiTypography>
                    </SuiBox>
                  </SuiBox>
                ),
                description: (
                  <SuiTypography variant="caption" color="text" fontWeight="medium" px={2}>
                    {item.description ? item.description : "n/a"}
                  </SuiTypography>
                ),
                provider: (
                  <SuiTypography variant="caption" color="secondary">
                    {ProviderName(item.providerId)}
                  </SuiTypography>
                ),
              }))
            }
            //page={1}
            //totalPages={1}
            //onPageChange={(event, value) => setPage(value)}
          />
        )}
        {isLoading && (
          <SuiTypography px={2} color="secondary">
            Loading..
          </SuiTypography>
        )}
        {error && (
          <SuiTypography px={2} color="error">
            Error occurred!
          </SuiTypography>
        )}
      </SuiBox>
    </Card>
  );
};

export default SecurityObjects;
