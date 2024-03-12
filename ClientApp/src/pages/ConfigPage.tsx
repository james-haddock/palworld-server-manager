import React from "react";
import { Text, TextInput, Button } from "@mantine/core";
import { gql, useQuery, useMutation } from "@apollo/client";

const GET_ALL_SETTINGS = gql`
  query {
    getAllSettings {
      key
      value
    }
  }
`;

const UPDATE_SETTING = gql`
  mutation UpdateSetting($key: String!, $value: String!) {
    updateServerSettings(key: $key, value: $value)
  }
`;

const ConfigPage: React.FC = () => {
  const { data, loading, error } = useQuery(GET_ALL_SETTINGS);
  const [updateSetting] = useMutation(UPDATE_SETTING);

  if (loading) return <Text>Loading...</Text>;
  if (error) return <Text>Error: {error.message}</Text>;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget as HTMLFormElement);
    for (const [key, value] of formData.entries()) {
      await updateSetting({ variables: { key, value } });
    }
    alert("Settings updated successfully");
  };

  return (
    <>
      <Text size="xl" fw={700}>
        Server Config
      </Text>
      <form onSubmit={handleSubmit}>
        {data.getAllSettings.map((setting: { key: string; value: string }) => (
          <div key={setting.key}>
            <Text>{setting.key}</Text>
            <TextInput name={setting.key} defaultValue={setting.value} />
          </div>
        ))}
        <Button type="submit">Save</Button>
      </form>
    </>
  );
};

export default ConfigPage;
